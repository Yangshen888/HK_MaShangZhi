<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.postjobs.query.totalCount"
        :pageSize="stores.postjobs.query.pageSize"
        :currentPage="stores.postjobs.query.currentPage"
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
                      v-model="stores.postjobs.query.kw"
                      placeholder="输入名称搜索..."
                      @on-search="handleSearchPostjobs()"
                    >
                      <Select
                        slot="prepend"
                        v-model="stores.postjobs.query.isDeleted"
                        @on-change="handleSearchPostjobs"
                        placeholder="删除状态"
                        style="width:60px;"
                      >
                        <Option
                          v-for="item in stores.postjobs.sources.isDeletedSources"
                          :value="item.value"
                          :key="item.value"
                        >{{item.text}}</Option>
                      </Select>
                      <Select
                        slot="prepend"
                        v-model="stores.postjobs.query.releaseState"
                        @on-change="handleSearchPostjobs"
                        placeholder="发布状态"
                        style="width:100px;"
                      >
                        <Option
                          v-for="item in stores.postjobs.sources.releaseStateSources"
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
                  title="新增岗位"
                >新增岗位</Button>
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
          :data="stores.postjobs.data"
          :columns="stores.postjobs.columns"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
        >
          <template slot-scope="{row,index}" slot="releaseState">
            <span>{{renderReleaseState(row.releaseState)}}</span>
          </template>
          <template slot-scope="{row,index}" slot="fullState">
            <span>{{renderFullState(row.fullState)}}</span>
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
                v-can="'edit'"
                type="primary"
                size="small"
                shape="circle"
                icon="md-create"
                @click="handleEdit(row)"
              ></Button>
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
            <FormItem label="单位名称" prop="unit">
              <Input
                v-model="formModel.fields.unit"
                placeholder="请输入单位名称"
                style="width: 400px"
                :maxlength="50"
              />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="岗位名称" prop="unitName">
              <Input
                v-model="formModel.fields.unitName"
                placeholder="请输入岗位名称"
                style="width: 400px"
                :maxlength="50"
              />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="工作地点" prop="site">
              <Input
                v-model="formModel.fields.site"
                placeholder="请输入工作地点"
                style="width: 400px"
                :maxlength="50"
              />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="用人要求" prop="require">
              <Input
                v-model="formModel.fields.require"
                placeholder="请输入用人要求"
                style="width: 400px"
                :maxlength="50"
              />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="招收人数" prop="num">
              <Input
                v-model="formModel.fields.num"
                @keyup.native="formModel.fields.num=formModel.fields.num.replace(/[^\d]/g,'')"
                placeholder="请输入招收人数"
                style="width: 400px"
                :maxlength="3"
              />
            </FormItem>
          </Col>
        </Row>

        <Row :gutter="16">
          <Col span="12">
            <FormItem label="发布状态" prop="isEnable" label-position="left">
              <i-switch
                size="large"
                v-model="formModel.fields.releaseState"
                :true-value="1"
                :false-value="0"
              >
                <span slot="open">启用</span>
                <span slot="close">禁用</span>
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
  </div>
</template>
<script>
  import DzTable from "_c/tables/dz-table.vue";
  import Tables from "_c/tables";
  import {
    getPripostjobsList,
    createPostjobs,
    loadPostjobs,
    editPostjobs,
    getPostjobsDel,
    batchCommand
  } from "@/api/workstudy/postjobs";
  export default {
    name: "postjobs",
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
      return {
        showdetails: false,
        details: "",
        commands: {
          delete: { name: "delete", title: "删除" },
          recover: { name: "recover", title: "恢复" },
          forbidden: { name: "forbidden", title: "禁用" },
          normal: { name: "normal", title: "启用" }
        },
        formModel: {
          opened: false,
          title: "创建类别",
          mode: "create",
          selection: [],
          fields: {
            unit: "",
            unitName: "",
            require: "",
            site:"",
            num:"",
            releaseState:0
          },
          rules: {
            unit: [{type: "string",required: true,message: "请输入单位名称",trigger:'blur'}],
            unitName: [{type: "string",required: true,message: "请输入岗位名称",trigger:'blur'}],
            site: [{type: "string",required: true,message: "请输入工作地点",trigger:'blur'}],
            require: [{type: "string",required: true,message: "请输入用人要求",trigger:'blur'}],
            num: [
              { required: true, message: "请输入人数" },
              { validator: checkNum, trigger: "blur" }],
          }
        },
        stores: {
          postjobs: {
            query: {
              totalCount: 0,
              pageSize: 20,
              currentPage: 1,
              kw: "",
              year: "",
              isDeleted: 0,
              status: -1,
              releaseState:-1,
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
                { value: 0, text: "未发布" },
                { value: 1, text: "已发布" }
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
              { title: "用工单位", key: "unit",ellipsis: true,tooltip:true },
              { title: "岗位名称", key: "unitName",ellipsis: true,tooltip:true },
              { title: "用人要求", key: "require",ellipsis: true,tooltip:true },
              { title: "工作地点", key: "site",ellipsis: true,tooltip:true },
              { title: "招收人数", key: "num",ellipsis: true,tooltip:true },
              { title: "发布状态", key: "releaseState",slot:"releaseState"},
              // { title: "是否启用", key: "isEnable"},
              { title: "招收状态", key: "fullState",slot:"fullState" },
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
          unit: "",
          unitName: "",
          require: "",
          site:"",
          num:"",
          releaseState:0
        }
      };
    },
    computed: {
      formTitle() {
        if (this.formModel.mode === "create") {
          return "新增岗位";
        }
        if (this.formModel.mode === "edit") {
          return "编辑岗位";
        }
        return "";
      },
      selectedRows() {
        return this.formModel.selection;
      },
      selectedRowsId() {
        return this.formModel.selection.map(x => x.postUuid);
      }
    },
    methods: {
      renderReleaseState(isEnable){
        let _status = "未知";
        switch(isEnable){
          case 0:
            _status= "未发布";
            break;
          case 1:
            _status= "已发布";
            break;
        }
        return _status;
      },
      renderFullState(isEnable){
        let _status = "未知";
        switch(isEnable){
          case 0:
            _status= "未满";
            break;
          case 1:
            _status= "已满";
            break;
        }
        return _status;
      },
      loadPripostjobsList() {
        getPripostjobsList(this.stores.postjobs.query).then(res => {
          this.stores.postjobs.data = res.data.data;
          this.stores.postjobs.query.totalCount = res.data.totalCount;
          //console.log(this.stores.educaplan.data);
        });
      },
      handleSearchPostjobs() {
        this.stores.postjobs.query.currentPage = 1;
        this.loadPripostjobsList();
      },
      handleRefresh() {
        this.stores.postjobs.query.currentPage = 1;
        this.loadPripostjobsList();
      },
      //创建，修改
      handleSubmitPlan() {
        let valid = this.validateForm();
        if (valid) {
          if (this.formModel.mode === "create") {
            this.docreatePostjobs();
          }
          if (this.formModel.mode === "edit") {
            this.doEditPlan();
          }
        }
      },
      docreatePostjobs() {
        createPostjobs(this.formModel.fields).then(res => {
          if (res.data.code === 200) {
            this.$Message.success(res.data.message);
            this.handleCloseFormWindow();
            this.loadPripostjobsList();
          } else {
            this.$Message.warning(res.data.message);
          }
        });
      },
      doEditPlan() {
        editPostjobs(this.formModel.fields).then(res => {
          if (res.data.code === 200) {
            this.$Message.success(res.data.message);
            this.handleCloseFormWindow();
            this.loadPripostjobsList();
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
            this.loadPripostjobsList();
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
      //单条删除
      handleDelete(row) {
        this.doDelete(row.postUuid);
      },
      doDelete(ids) {
        if (!ids) {
          this.$Message.warning("请选择至少一条数据");
          return;
        }
        getPostjobsDel(ids).then(res => {
          if (res.data.code === 200) {
            this.$Message.success(res.data.message);
            this.loadPripostjobsList();
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
        this.doLoadPostjobs(row.postUuid);
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
        this.formModel.fields = JSON.parse(JSON.stringify(this.initdatacopy));
        //this.$refs["formPlan"].resetFields();
      },
      doLoadPostjobs(guid) {
        loadPostjobs({ guid: guid }).then(res => {
          this.formModel.fields = res.data.data;
        });
      },
      handlePageChanged(page) {
        this.stores.postjobs.query.currentPage = page;
        this.loadPripostjobsList();
      },
      handlePageSizeChanged(pageSize) {
        this.stores.postjobs.query.pageSize = pageSize;
        this.loadPripostjobsList();
      }
    },
    mounted() {
      this.loadPripostjobsList();
    }
  };
</script>
<style scoped>
</style>
