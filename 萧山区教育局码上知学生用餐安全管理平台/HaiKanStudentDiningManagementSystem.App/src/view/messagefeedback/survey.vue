<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.survey.query.totalCount"
        :pageSize="stores.survey.query.pageSize"
        :currentPage="stores.survey.query.currentPage"
        @on-page-change="handlePageChanged"
        @on-page-size-change="handlePageSizeChanged"
      >
        <div slot="searcher">
          <section class="dnc-toolbar-wrap">
            <Row :gutter="16">
              <Col span="16">
                <Form inline @submit.native.prevent>
                  <FormItem>
                    <Input
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.survey.query.kw"
                      placeholder="输入名称搜索..."
                      @on-search="handleSearchSurvey()"
                    >
                      <Select
                        slot="prepend"
                        v-model="stores.survey.query.isDeleted"
                        @on-change="handleSearchSurvey"
                        placeholder="删除状态"
                        style="width:60px;"
                      >
                        <Option
                          v-for="item in stores.survey.sources.isDeletedSources"
                          :value="item.value"
                          :key="item.value"
                        >{{item.text}}</Option>
                      </Select>
                      <Select
                        slot="prepend"
                        v-model="stores.survey.query.productState"
                        @on-change="handleSearchSurvey"
                        placeholder="生成状态"
                        style="width:100px;"
                      >
                        <Option
                          v-for="item in stores.survey.sources.releaseStateSources"
                          :value="item.value"
                          :key="item.value"
                        >{{item.text}}</Option>
                      </Select>
                      <Select
                        slot="prepend"
                        v-model="stores.survey.query.proceedState"
                        @on-change="handleSearchSurvey"
                        placeholder="进行状态"
                        style="width:100px;"
                      >
                        <Option
                          v-for="item in stores.survey.sources.proceedStateSources"
                          :value="item.value"
                          :key="item.value"
                        >{{item.text}}</Option>
                      </Select>
                    </Input>
                  </FormItem>
                </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                  <Button
                    v-can="'delete'"
                    class="txt-danger"
                    icon="md-trash"
                    title="删除"
                    @click="handleBatchCommand('delete')"
                  ></Button>
                  <Button
                    class="txt-success"
                    icon="md-redo"
                    title="恢复"
                    @click="handleBatchCommand('recover')"
                  ></Button>

                  <Button icon="md-refresh" title="刷新" @click="handleRefresh"></Button>
                </ButtonGroup>
                <Button
                  v-can="'create'"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="新增问卷"
                >新增问卷</Button>
              </Col>
            </Row>
          </section>
        </div>
        <Table
          slot="table"
          ref="tables"
          :border="false"
          size="small"
          :highlight-row="true"
          :data="stores.survey.data"
          :columns="stores.survey.columns"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
        >
          <template slot-scope="{row,index}" slot="proceedState">
            <span>{{renderProceedState(row.proceedState)}}</span>
          </template>
          <template slot-scope="{row,index}" slot="productState">
            <span>{{renderProductState(row.productState)}}</span>
          </template>
          <template slot-scope="{row,index}" slot="beginTime">
            <span>{{renderDate(row.beginTime)}}</span>
          </template>
          <template slot-scope="{row,index}" slot="endTime">
            <span>{{renderDate(row.endTime)}}</span>
          </template>
          <!--          <template slot-scope="{ row, index }" slot="detail">-->
          <!--            <Tooltip placement="top" content="详情" :delay="1000" :transfer="true">-->
          <!--              <Button-->
          <!--                type="primary"-->
          <!--                size="small"-->
          <!--                shape="circle"-->
          <!--                icon="md-search"-->
          <!--                @click="handleShow(row)"-->
          <!--              ></Button>-->
          <!--            </Tooltip>-->
          <!--          </template>-->
          <template slot-scope="{ row, index }" slot="action">
            <Poptip confirm :transfer="true" title="确定要删除吗?" @on-ok="handleDelete(row)">
              <Tooltip placement="top" content="删除" :delay="1000" :transfer="true">
                <Button v-can="'delete'" type="error" size="small" shape="circle" icon="md-trash"></Button>
              </Tooltip>
            </Poptip>
            <Tooltip placement="top" content="编辑" :delay="1000" :transfer="true">
              <Button
                v-if="row.productState==0"
                v-can="'edit'"
                type="primary"
                size="small"
                shape="circle"
                icon="md-create"
                @click="handleEdit(row)"
              ></Button>
            </Tooltip>
            <Poptip confirm :transfer="true" title="确定要开始/结束吗?" @on-ok="handleOpenOrClose(row)">
              <Tooltip placement="top" content="开始/结束" :delay="1000" :transfer="true">
                <Button v-can="'open'" type="success" size="small" shape="circle" icon="ios-hand"></Button>
              </Tooltip>
            </Poptip>
            <Tooltip placement="top" content="详情" :delay="1000" :transfer="true">
              <Button v-can="'detail'" type="warning" size="small" shape="circle" icon="md-search" @click="handleDetail(row)"></Button>
            </Tooltip>
          </template>
        </Table>
      </dz-table>
    </Card>
    <Drawer
      :title="formTitle"
      v-model="formModel.opened"
      width="500"
      :mask-closable="false"
      :mask="false"
      :styles="styles"
    >
      <Form
        v-if="formModel.opened"
        :model="formModel.fields"
        ref="formPlan"
        :rules="formModel.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="问卷标题" prop="headline">
              <Input
                v-model.trim="formModel.fields.headline"
                placeholder="请输入问卷标题"
                style="width: 400px"
                :maxlength="50"
              />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="问卷类型" prop="type">
              <Input
                v-model.trim="formModel.fields.type"
                placeholder="请输入问卷类型"
                style="width: 400px"
                :maxlength="50"
              />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="开始时间" prop="beginTime">
              <DatePicker type="datetime" format="yyyy-MM-dd HH:mm" @on-change="formModel.fields.beginTime=$event"
                          :value="formModel.fields.beginTime" placeholder="开始时间" style="width: 400px"
                          :editable="false"/>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="结束时间" prop="endTime">
              <DatePicker type="datetime" format="yyyy-MM-dd HH:mm" @on-change="formModel.fields.endTime=$event"
                          :value="formModel.fields.endTime" placeholder="结束时间" style="width: 400px"
                          :editable="false"/>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
            <FormItem label="问题设置">
              <Button type="info" @click="addquestion">添加</Button>
              <Table
                ref="tables1"
                :border="false"
                size="small"
                :highlight-row="true"
                :data="stores.surveyquestion.data"
                :columns="stores.surveyquestion.columns"
              >
                <template slot-scope="{ row, index }" slot="questionType">
                  <span>{{renderQuestionType(row.questionType)}}</span>
                </template>
                <template slot-scope="{ row, index }" slot="isMuti">
                  <span>{{renderIsMuti(row.isMuti)}}</span>
                </template>
                <template slot-scope="{ row, index }" slot="action">
                  <Poptip confirm :transfer="true" title="确定要删除吗?" @on-ok="handleDeleteQuestion(row)">
                    <Tooltip placement="top" content="删除" :delay="1000" :transfer="true">
                      <Button
                        type="error"
                        size="small"
                        shape="circle"
                        icon="md-trash"
                      ></Button>
                    </Tooltip>
                  </Poptip>
                  <Tooltip
                    placement="top"
                    content="编辑"
                    :delay="1000"
                    :transfer="true"
                  >
                    <Button
                      type="primary"
                      size="small"
                      shape="circle"
                      icon="md-create"
                      @click="handleEditQuestion(row)"
                    ></Button>
                  </Tooltip>
                </template>
              </Table>
            </FormItem>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="生成状态" prop="productState" label-position="left">
              <i-switch
                size="large"
                v-model="formModel.fields.productState"
                :true-value="1"
                :false-value="0"
              >
                <span slot="open">生成</span>
                <span slot="close">未成</span>
              </i-switch>
            </FormItem>
          </Col>
        </Row>
      </Form>

      <div style="margin-top: 100px">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitPlan">保 存</Button>
        <Button style="margin-left: 8px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>
    <Drawer
      :title="'具体问题'"
      v-model="formModelquestion.opened"
      width="450"
      :mask-closable="false"
      :mask="false"
      :styles="styles"
    >
      <Form
        v-if="formModelquestion.opened"
        :model="formModelquestion.fields"
        ref="formquestion"
        :rules="formModelquestion.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="题目类型" prop="questionType">
              <Select v-model="formModelquestion.fields.questionType" :disabled="formModelquestion.isQuestiontypedisable">
                <Option v-for="item in formModelquestion.source.type"
                        :value="item.questionType"
                        :key="item.questionType"
                        >
                  {{item.typeName}}
                </Option>
              </Select>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="是否多选" prop="isMuti">
              <Select v-model="formModelquestion.fields.isMuti" :disabled="formModelquestion.isMutidisable">
                <Option v-for="item in formModelquestion.source.mutype"
                        :value="item.isMuti"
                        :key="item.isMuti"
                        >
                  {{item.Name}}
                </Option>
              </Select>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="题目内容" prop="questionTitle">
              <Input
                v-model="formModelquestion.fields.questionTitle"
                placeholder="题目内容"
                :maxlength="50"
              />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16" v-if="formModelquestion.fields.questionType==0">
          <FormItem label="选项设置">
            <Button type="info" @click="addquestionitem" >添加</Button>
            <Table
              ref="tables1"
              :border="false"
              size="small"
              :highlight-row="true"
              :data="stores.surveyquestionitem.data"
              :columns="stores.surveyquestionitem.columns"
            >
              <template slot-scope="{ row, index }" slot="action">
                <Poptip confirm :transfer="true" title="确定要删除吗?" @on-ok="handleDeleteQuestionItem(row)">
                  <Tooltip placement="top" content="删除" :delay="1000" :transfer="true">
                    <Button
                      type="error"
                      size="small"
                      shape="circle"
                      icon="md-trash"
                    ></Button>
                  </Tooltip>
                </Poptip>
                <Tooltip
                  placement="top"
                  content="编辑"
                  :delay="1000"
                  :transfer="true"
                >
                  <Button
                    type="primary"
                    size="small"
                    shape="circle"
                    icon="md-create"
                    @click="handleEditQuestionItem(row)"
                  ></Button>
                </Tooltip>
              </template>
            </Table>
          </FormItem>
        </Row>

      </Form>

      <div style="margin-top: 50px">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitQuestion">保 存</Button>
        <Button style="margin-left: 8px" icon="md-close" @click="formModelquestion.opened = false">取 消</Button>
      </div>
    </Drawer>
    <Drawer
      :title="'具体选项'"
      v-model="formModelquestionitem.opened"
      width="400"
      :mask-closable="false"
      :mask="false"
      :styles="styles"
    >
      <Form
        v-if="formModelquestionitem.opened"
        :model="formModelquestionitem.fields"
        ref="formquestionitem"
        :rules="formModelquestionitem.rules"
        label-position="top"
      >


        <Row :gutter="16">
          <Col span="12">
            <FormItem label="选项序号" prop="optionts">
              <Input
                v-model="formModelquestionitem.fields.optionts"
                placeholder="选项序号"
                :maxlength="5"
              />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="选项内容" prop="questionStr">
              <Input
                v-model="formModelquestionitem.fields.questionStr"
                placeholder="选项内容"
                :maxlength="50"
              />
            </FormItem>
          </Col>
        </Row>

      </Form>

      <div style="margin-top: 50px">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitQuestionItem">保 存</Button>
        <Button style="margin-left: 8px" icon="md-close" @click="formModelquestionitem.opened = false">取 消</Button>
      </div>
    </Drawer>
    <Drawer
      :title="'问卷统计'"
      v-model="formModeldetail.opened"
      width="400"
      :mask-closable="false"
      :mask="false"
      :styles="styles"

    >

      <div>
      <div>{{"问卷标题: "}}{{detaildata.headline}}</div>
      <div>{{"问卷类型: "}}{{detaildata.type}}</div>
      <div v-for="(item,index) in detaildata.surveyQuestionDetail" style="padding: 10px">

        <div v-if="item.questionType==0&&item.isMuti==0">
          <div>{{(index+1)+'. '}}{{item.questionTitle}}{{'(单选题)'}}</div>
          <div v-for="(item1,index1) in item.surveyQuestionItemDetail" style="padding: 5px">
            {{item1.optionts}}{{'. '}}{{item1.questionStr}}{{' （人数：'+item1.num+'）'}}
          </div>
        </div>

        <div v-else-if="item.questionType==0&&item.isMuti==1">
          <div>{{(index+1)+'. '}}{{item.questionTitle}}{{'(多选题)'}}</div>
          <div v-for="(item1,index1) in item.surveyQuestionItemDetail" style="padding: 5px">
            {{item1.optionts}}{{'. '}}{{item1.questionStr}}{{' （人数：'+item1.num+'）'}}
          </div>
        </div>

        <div v-else-if="item.questionType==1">
          <div>{{(index+1)+'. '}}{{item.questionTitle}}{{'(主观题)'}}</div>
          <Collapse  style="padding: 5px">
            <Panel name="1">
              {{item.subText.substring(0,9)}}
              <p slot="content">{{item.subText}}</p>
            </Panel>
          </Collapse>
        </div>

      </div>
      </div>
    </Drawer>
  </div>
</template>
<script>
  import DzTable from "_c/tables/dz-table.vue";
  import Tables from "_c/tables";
  import {
    getSurveyList,
    createSurvey,
    OpenOrClose,
    loadSurveyDetail,
    loadSurvey,
    editSurvey,
    getSurveyDel,
    batchCommand,

    getSurveyQuestionList,
    createSurveyQuestion,
    loadSurveyQuestion,
    editSurveyQuestion,
    getSurveyQuestionDel,

    getSurveyQuestionItemList,
    createSurveyQuestionItem,
    loadSurveyQuestionItem,
    editSurveyQuestionItem,
    getSurveyQuestionItemDel,
  } from "@/api/messagefeedback/survey";
  export default {
    name: "survey",
    components: {
      Tables,
      DzTable
    },
    data() {
      let checkNum = (rule, value, callback) => {
        if (value === "") {
          callback(new Error("请输入"));
        } else if (value <= 0 || value > 999) {
          callback(new Error("请输入1-999的数字"));
        } else {
          callback();
        }
      };
      let checkEndDate = (rule, value, callback,source) => {

        if(this.formModel.fields.beginTime=="")
        {
          callback(new Error("请先选择开始时间"));
        }
        else if(this.formModel.fields.endTime<this.formModel.fields.beginTime)
        {
          callback(new Error("结束时间必须大于开始时间"));
        }

        callback();
      };
      return {
        surveyUuid:'',
        surveyQuestionsUuid:'',
        commands: {
          delete: { name: "delete", title: "删除" },
          recover: { name: "recover", title: "恢复" },
          forbidden: { name: "forbidden", title: "禁用" },
          normal: { name: "normal", title: "启用" }
        },
        formModeldetail:{
          opened: false
        },
        formModelquestion:{
          isMutidisable:true,
          isQuestiontypedisable:true,
          adddisable:true,
          opened: false,
          mode: "create",
          fields:{
            surveyUuid:'',
            surveyQuestionsUuid:'',
            questionTitle:'',
            questionType:0,
            isMuti:0
          },
          rules: {
            questionTitle: [{type: "string",required: true,message: "请输入题目内容",trigger:'blur'}],
          },
          source:{
            type:[
              {questionType:0,typeName:'客观题'},
              {questionType:1,typeName:'主观题'}
            ],
            mutype:[
              {isMuti:0,Name:'否'},
              {isMuti:1,Name:'是'}
            ]
          }
        },
        formModelquestionitem:{
          opened: false,
          mode: "create",
          fields:{
            surveyQuestionsUuid:'',
            optionts:'',
            questionStr:''
          },
          rules: {
            optionts: [{type: "string",required: true,message: "请输入选项序号",trigger:'blur'}],
            questionStr: [{type: "string",required: true,message: "请输入选项内容",trigger:'blur'}],
          },
        },
        formModel: {
          opened: false,
          title: "创建类别",
          mode: "create",
          selection: [],
          fields: {
            surveyUuid:'',
            headline: "",
            type: "",
            beginTime: "",
            endTime:"",
            productState:-1
          },
          rules: {
            headline: [{type: "string",required: true,message: "请输入单位名称",trigger:'blur'}],
            type: [{type: "string",required: true,message: "请输入岗位名称",trigger:'blur'}],
            beginTime:[{required: true, message: "请选择时间", trigger: 'blur'}],
            endTime:[{required: true, message: "请选择时间", trigger: 'blur'},{validator: checkEndDate }],
          }
        },
        stores: {
          survey: {
            query: {
              totalCount: 0,
              pageSize: 20,
              currentPage: 1,
              kw: "",
              productState:-1,
              proceedState:-1,
              isDeleted: 0,
              sort: [
                {
                  direct: "DESC",
                  field: "ID"
                }
              ]
            },
            sources: {
              releaseStateSources: [
                { value: -1, text: "全部" },
                { value: 0, text: "未生成" },
                { value: 1, text: "已生成" }
              ],
              proceedStateSources:[
                  { value: -1, text: "全部" },
                  { value: 0, text: "未开始" },
                  { value: 1, text: "进行中" },
                { value: 2, text: "已结束" }
                ],
              isDeletedSources: [
                { value: -1, text: "全部" },
                { value: 0, text: "正常" },
                { value: 1, text: "已删" }
              ],
              statusSources: [
                { value: -1, text: "全部" },
                { value: 0, text: "禁用" },
                { value: 1, text: "正常" }
              ],
              statusFormSources: [
                { value: 0, text: "禁用" },
                { value: 1, text: "正常" }
              ]
            },
            columns: [
              { type: "selection", width: 50, key: "handle" },
              { title: "问卷标题", key: "headline",ellipsis: true,tooltip:true },
              { title: "问卷类型", key: "type",ellipsis: true,tooltip:true },
              { title: "所属学校", key: "schoolName",ellipsis: true ,tooltip:true},
              { title: "开始时间", key: "beginTime",slot:"beginTime" },
              { title: "结束时间", key: "endTime",slot:"endTime" },
              { title: "进行状态", key: "proceedState",ellipsis: true,slot:"proceedState",tooltip:true},
              // { title: "是否启用", key: "isEnable"},
              { title: "生成状态", key: "productState",ellipsis: true,slot:"productState" ,tooltip:true},
              { title: "已答题人数", key: "number",ellipsis: true,tooltip:true },
              {
                title: "操作",
                fixed: "right",
                align: "center",
                width: 150,
                className: "table-command-column",
                slot: "action"
              }
            ],
            data: []
          },
          surveyquestion:{
            columns: [
              { type: "selection", width: 50, key: "handle" },
              { title: "问题内容", key: "questionTitle",ellipsis: true,tooltip:true },
              { title: "问题类型", key: "questionType",ellipsis: true,tooltip:true ,slot: 'questionType'},
              { title: "是否多选", key: "isMuti",ellipsis: true,tooltip:true,slot: 'isMuti' },
              {
                title: "操作",
                fixed: "right",
                align: "center",
                width: 150,
                className: "table-command-column",
                slot: "action"
              }
            ],
            data: []
          },
          surveyquestionitem:{
            columns: [
              { type: "selection", width: 50, key: "handle" },
              { title: "选项序号", key: "optionts",ellipsis: true,tooltip:true },
              { title: "选项内容", key: "questionStr",ellipsis: true,tooltip:true },
              {
                title: "操作",
                fixed: "right",
                align: "center",
                width: 150,
                className: "table-command-column",
                slot: "action"
              }
            ],
            data: []
          }
        },
        styles: {
          height: "calc(100% - 55px)",
          overflow: "auto",
          paddingBottom: "53px",
          position: "static"
        },
        initdatacopy: {
          surveyUuid:'',
          headline: "",
          type: "",
          beginTime: "",
          endTime:"",
          productState:0
        },
        initdataquestioncopy:{
          surveyUuid:'',
          surveyQuestionsUuid:'',
          questionTitle:'',
          questionType:0,
          isMuti:0
        },
        initdataquestionitemcopy:{
          surveyQuestionsUuid:'',
          optionts:'',
          questionStr:''
        },
        detaildata:{

        }
      };
    },
    computed: {
      formTitle() {
        if (this.formModel.mode === "create") {
          return "新增问卷";
        }
        if (this.formModel.mode === "edit") {
          return "编辑问卷";
        }
        return "";
      },
      selectedRows() {
        return this.formModel.selection;
      },
      selectedRowsId() {
        return this.formModel.selection.map(x => x.surveyUuid);
      }
    },
    methods: {
      /*
     问卷题目相关操作
     */
      renderQuestionType(state){
        let _status = "未知";
        switch(state){
          case 0:
            _status= "客观题";
            break;
          case 1:
            _status= "主观题";
            break;
        }
        return _status;
      },
      renderIsMuti(state){
        let _status = "未知";
        switch(state){
          case 0:
            _status= "否";
            break;
          case 1:
            _status= "是";
            break;
        }
        return _status;
      },
      addquestion(){
        this.handleResetFormQuestion();
        if(this.formModel.mode === "create")
        {
          if(this.surveyUuid=='')
          {
            let valid = this.validateForm();
            if (valid)
            {
              this.docreateSurveyinit();
            }
          }
        }
        if(this.formModel.mode === "edit")
        {
          if(this.surveyUuid!='')
          {
            this.formModelquestion.mode="create";
            this.formModelquestion.isQuestiontypedisable=false;
            this.formModelquestion.isMutidisable=false;
            this.formModelquestion.opened = true;
          }
        }
      },
      dogetSurveyQuestionList(){
        getSurveyQuestionList({guid:this.surveyUuid}).then(res=>{
          if(res.data.code==200)
          {
            this.stores.surveyquestion.data = res.data.data;
          }
        })
      },
      docreateSurveyinit() {
        createSurvey(this.formModel.fields).then(res => {
          if (res.data.code === 200) {
            this.surveyUuid = res.data.data.SurveyUuid;
            this.$Message.success(res.data.message);
            this.loadSurveyList();
            this.formModelquestion.mode="create";
            this.formModel.mode="edit";
            this.formModelquestion.isQuestiontypedisable=false;
            this.formModelquestion.isMutidisable=false;
            this.formModelquestion.opened = true;
          } else {
            this.$Message.warning(res.data.message);
          }
        });
      },
      validateFormQuestion() {
        let _valid = false;
        this.$refs["formquestion"].validate(valid => {
          if (!valid) {
            this.$Message.error("请完善表单信息");
          } else {
            _valid = true;
          }
        });
        return _valid;
      },
      handleSubmitQuestion(){
        let valid = this.validateFormQuestion();
        if (valid) {
          if (this.formModelquestion.mode === "create") {
            this.docreateSurveyQuestion();
          }
          if (this.formModelquestion.mode === "edit") {
            this.doeditSurveyQuestion();
          }
        }
      },
      docreateSurveyQuestioninit(){
        this.formModelquestion.fields.surveyUuid=this.surveyUuid;
        createSurveyQuestion(this.formModelquestion.fields).then(res=>{
          if(res.data.code==200)
          {
            this.surveyQuestionsUuid = res.data.data.SurveyQuestionsUuid;
            this.dogetSurveyQuestionList();
            this.formModelquestion.isQuestiontypedisable=true;
            this.formModelquestion.isMutidisable=true;
            this.$Message.success(res.data.message);
            this.formModelquestion.mode="edit";
            this.formModelquestionitem.mode="create";
            this.formModelquestionitem.opened = true;
          }
          else
          {
            this.$Message.warning(res.data.message);
          }
        })
      },
      docreateSurveyQuestion(){
        this.formModelquestion.fields.surveyUuid=this.surveyUuid;
        createSurveyQuestion(this.formModelquestion.fields).then(res=>{
          if(res.data.code==200)
          {
            this.surveyQuestionsUuid = res.data.data.SurveyQuestionsUuid;
            this.dogetSurveyQuestionList();
            this.$Message.success(res.data.message);
            this.formModelquestion.opened=false;
          }
          else
          {
            this.$Message.warning(res.data.message);
          }
        })
      },
      doeditSurveyQuestion(){
        if(this.formModelquestion.fields.surveyQuestionsUuid=='')
        {
          this.formModelquestion.fields.surveyQuestionsUuid=this.surveyQuestionsUuid;
        }
        editSurveyQuestion(this.formModelquestion.fields).then(res=>{
          if(res.data.code==200)
          {
            this.dogetSurveyQuestionList();
            this.$Message.success(res.data.message);
            this.formModelquestion.opened=false;
          }
          else
          {
            this.$Message.warning(res.data.message);
          }
        })
      },
      handleEditQuestion(row){
        this.handleResetFormQuestion();
        this.formModelquestion.mode="edit";
        this.formModelquestion.isQuestiontypedisable=true;
        this.formModelquestion.isMutidisable=true;
        this.formModelquestion.opened=true;
        this.doLoadSurveyQuestion(row.surveyQuestionsUuid);

      },
      handleResetFormQuestion(){
        this.surveyQuestionsUuid='';
        this.stores.surveyquestionitem.data=[];
        this.formModelquestion.fields = JSON.parse(JSON.stringify(this.initdataquestioncopy));
      },
      doLoadSurveyQuestion(guid)
      {
        loadSurveyQuestion({guid:guid}).then(res=>{
          if(res.data.code==200)
          {
            this.formModelquestion.fields=res.data.data;
            this.surveyQuestionsUuid = res.data.data.surveyQuestionsUuid;
            this.dogetSurveyQuestionItemList();
          }
          else
          {
            this.$Message.warning(res.data.message);
          }
        })
      },
      //单条删除
      handleDeleteQuestion(row) {
        this.doDeleteQuestion(row.surveyQuestionsUuid);
      },
      doDeleteQuestion(ids) {
        if (!ids) {
          this.$Message.warning("请选择至少一条数据");
          return;
        }
        getSurveyQuestionDel(ids).then(res => {
          if (res.data.code === 200) {
            this.$Message.success(res.data.message);
            this.dogetSurveyQuestionList();
          } else {
            this.$Message.warning(res.data.message);
          }
        });
      },
      /*
     问卷题目选项相关操作
     */
      addquestionitem(){
        this.handleResetFormQuestionItem();
        if(this.formModelquestion.mode === "create")
        {
          if(this.surveyQuestionsUuid=='')
          {
            let valid = this.validateFormQuestion();
            if (valid)
            {
              this.docreateSurveyQuestioninit();
            }
          }
        }
        if(this.formModelquestion.mode === "edit")
        {
          if(this.surveyQuestionsUuid!='')
          {
            this.formModelquestionitem.mode="create";
            this.formModelquestionitem.opened = true;
          }
        }
      },
      dogetSurveyQuestionItemList(){
        getSurveyQuestionItemList({guid:this.surveyQuestionsUuid}).then(res=>{
          if(res.data.code==200)
          {
            this.stores.surveyquestionitem.data = res.data.data;
          }
        })
      },
      handleResetFormQuestionItem(){
        this.formModelquestionitem.fields = JSON.parse(JSON.stringify(this.initdataquestionitemcopy));
      },
      validateFormQuestionItem() {
        let _valid = false;
        this.$refs["formquestionitem"].validate(valid => {
          if (!valid) {
            this.$Message.error("请完善表单信息");
          } else {
            _valid = true;
          }
        });
        return _valid;
      },
      handleSubmitQuestionItem(){
        let valid = this.validateFormQuestionItem();
        if (valid) {
          if (this.formModelquestionitem.mode === "create") {
            this.docreateSurveyQuestionItem();
          }
          if (this.formModelquestionitem.mode === "edit") {
            this.doeditSurveyQuestionItem();
          }
        }
      },
      docreateSurveyQuestionItem(){
        this.formModelquestionitem.fields.surveyQuestionsUuid=this.surveyQuestionsUuid;
        createSurveyQuestionItem(this.formModelquestionitem.fields).then(res=>{
          if(res.data.code==200)
          {
            this.dogetSurveyQuestionItemList();
            this.$Message.success(res.data.message);
            this.formModelquestionitem.opened=false;
          }
          else
          {
            this.$Message.warning(res.data.message);
          }
        })
      },
      handleEditQuestionItem(row){
        this.handleResetFormQuestionItem();
        this.formModelquestionitem.mode="edit";
        this.formModelquestionitem.opened=true;
        this.doLoadSurveyQuestionItem(row.surveyQuestionsItemsUuid);
      },
      doLoadSurveyQuestionItem(guid)
      {
        loadSurveyQuestionItem({guid:guid}).then(res=>{
          if(res.data.code==200)
          {
            this.formModelquestionitem.fields=res.data.data;
          }
          else
          {
            this.$Message.warning(res.data.message);
          }
        })
      },
      doeditSurveyQuestionItem(){
        editSurveyQuestionItem(this.formModelquestionitem.fields).then(res=>{
          if(res.data.code==200)
          {
            this.dogetSurveyQuestionItemList();
            this.$Message.success(res.data.message);
            this.formModelquestionitem.opened=false;
          }
          else
          {
            this.$Message.warning(res.data.message);
          }
        })
      },
     //单条删除
      handleDeleteQuestionItem(row) {
        this.doDeleteQuestionItem(row.surveyQuestionsUuid);
      },
      doDeleteQuestionItem(ids) {
        if (!ids) {
          this.$Message.warning("请选择至少一条数据");
          return;
        }
        getSurveyQuestionItemDel(ids).then(res => {
          if (res.data.code === 200) {
            this.$Message.success(res.data.message);
            this.dogetSurveyQuestionItemList();
          } else {
            this.$Message.warning(res.data.message);
          }
        });
      },

      /*
      问卷列表相关操作
      */
      renderProceedState(state){
        let _status = "未知";
        switch(state){
          case 0:
            _status= "未开始";
            break;
          case 1:
            _status= "进行中";
            break;
          case 2:
            _status= "已结束";
            break;
        }
        return _status;
      },
      renderProductState(state){
        let _status = "未知";
        switch(state){
          case 0:
            _status= "未生成";
            break;
          case 1:
            _status= "已生成";
            break;
        }
        return _status;
      },
      renderDate(date){
        return this.$utils.Time(date);
      },
      loadSurveyList() {
        getSurveyList(this.stores.survey.query).then(res => {
          this.stores.survey.data = res.data.data;
          this.stores.survey.query.totalCount = res.data.totalCount;
          //console.log(this.stores.educaplan.data);
        });
      },
      handleSearchSurvey() {
        this.stores.survey.query.currentPage = 1;
        this.loadSurveyList();
      },
      handleRefresh() {
        this.stores.survey.query.currentPage = 1;
        this.loadSurveyList();
      },
      //创建，修改
      handleSubmitPlan() {
        let valid = this.validateForm();
        if (valid) {
          if (this.formModel.mode === "create") {
            this.docreateSurvey();
          }
          if (this.formModel.mode === "edit") {
            this.doEditPlan();
          }
        }
      },
      docreateSurvey() {
        createSurvey(this.formModel.fields).then(res => {
          if (res.data.code === 200) {
            this.$Message.success(res.data.message);
            this.handleCloseFormWindow();
            this.loadSurveyList();
          } else {
            this.$Message.warning(res.data.message);
          }
        });
      },
      doEditPlan() {
        if(this.formModel.fields.surveyUuid=='')
        {
          this.formModel.fields.surveyUuid=this.surveyUuid;
        }
        editSurvey(this.formModel.fields).then(res => {
          if (res.data.code === 200) {
            this.$Message.success(res.data.message);
            this.handleCloseFormWindow();
            this.loadSurveyList();
          } else {
            this.$Message.warning(res.data.message);
          }
        });
      },
      validateForm() {
        let _valid = false;
        this.$refs["formPlan"].validate(valid => {
          if (!valid) {
            this.$Message.error("请完善表单信息");
          } else {
            _valid = true;
          }
        });
        return _valid;
      },
      //批量操作
      handleBatchCommand(command) {
        if (!this.selectedRowsId || this.selectedRowsId.length <= 0) {
          this.$Message.warning("请选择至少一条数据");
          return;
        }
        this.$Modal.confirm({
          title: "操作提示",
          content:
            "<p>确定要执行当前 [" +
            this.commands[command].title +
            "] 操作吗?</p>",
          loading: true,
          onOk: () => {
            this.doBatchCommand(command);
          }
        });
        //addsystemlog("delete","删除年度市级招生方案列表");
      },
      doBatchCommand(command) {
        batchCommand({
          command: command,
          ids: this.selectedRowsId.join(",")
        }).then(res => {
          if (res.data.code === 200) {
            this.$Message.success(res.data.message);
            this.loadSurveyList();
            this.formModel.selection = [];
          } else {
            this.$Message.warning(res.data.message);
          }
          this.$Modal.remove();
        });
      },
      handleSelectionChange(selection) {
        this.formModel.selection = selection;
      },
      rowClsRender(row, index) {
        if (row.isDeleted) {
          return "table-row-disabled";
        }
        return "";
      },
      handleOpenOrClose(row){
        OpenOrClose({guid:row.surveyUuid}).then(res=>{
          if(res.data.code==200)
          {
            this.loadSurveyList();
          }
          else
          {
            this.$Message.warning(res.data.message)
          }
        })

      },
      //单条删除
      handleDelete(row) {
        this.doDelete(row.surveyUuid);
      },
      doDelete(ids) {
        if (!ids) {
          this.$Message.warning("请选择至少一条数据");
          return;
        }
        getSurveyDel(ids).then(res => {
          if (res.data.code === 200) {
            this.$Message.success(res.data.message);
            this.loadSurveyList();
            this.formModel.selection = [];
          } else {
            this.$Message.warning(res.data.message);
          }
        });
      },
      //控制弹出子窗体
      handleOpenFormWindow() {
        this.formModel.opened = true;
      },
      handleCloseFormWindow() {
        this.formModel.opened = false;
      },
      //编辑
      handleEdit(row) {
        this.handleSwitchFormModeToEdit();
        this.handleResetFormRole();
        this.doLoadSurvey(row.surveyUuid);
      },

      handleShowCreateWindow() {
        this.handleSwitchFormModeToCreate();
        this.handleResetFormRole();
      },
      handleSwitchFormModeToCreate() {
        this.formModel.mode = "create";
        this.handleOpenFormWindow();
      },
      handleSwitchFormModeToEdit() {
        this.formModel.mode = "edit";
        this.handleOpenFormWindow();
      },
      handleSwitchFormModeToShow() {
        this.showdetails = true;
      },
      handleResetFormRole() {
        this.surveyUuid='';
        this.stores.surveyquestion.data=[];
        this.formModel.fields = JSON.parse(JSON.stringify(this.initdatacopy));
        //this.$refs["formPlan"].resetFields();
      },
      doLoadSurvey(guid) {
        loadSurvey({ guid: guid }).then(res => {
          this.surveyUuid = res.data.data.surveyUuid;
          this.formModel.fields = res.data.data;
          this.dogetSurveyQuestionList();
        });
      },
      handlePageChanged(page) {
        this.stores.survey.query.currentPage = page;
        this.loadSurveyList();
      },
      handlePageSizeChanged(pageSize) {
        this.stores.survey.query.pageSize = pageSize;
        this.loadSurveyList();
      },
      handleDetail(row){
        loadSurveyDetail({guid:row.surveyUuid}).then(res=>{
          if(res.data.code==200)
          {
            this.detaildata = res.data.data;
            this.formModeldetail.opened=true;
            //console.log(res.data.data);
          }
          else
          {
            this.$Message.warning(res.data.message);
          }
        })
      }
    },
    mounted() {
      this.loadSurveyList();
    }
  };
</script>
<style scoped>
</style>
