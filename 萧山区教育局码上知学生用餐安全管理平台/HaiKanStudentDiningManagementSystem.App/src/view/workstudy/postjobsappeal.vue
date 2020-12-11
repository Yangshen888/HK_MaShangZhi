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
                      placeholder="输入单位/岗位/姓名搜索..."
                      @on-search="handleSearchPostjobs()"
                    >
                      <Select
                        slot="prepend"
                        v-model="stores.postjobs.query.state"
                        @on-change="handleSearchPostjobs"
                        placeholder="审核状态"
                        style="width:100px;"
                      >
                        <Option
                          v-for="item in stores.postjobs.sources.stateSources"
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
                  <Button icon="md-refresh" title="刷新" @click="handleRefresh"></Button>
                </ButtonGroup>
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
          <template slot-scope="{row,index}" slot="poorState">
            <span>{{renderPoorState(row.poorState)}}</span>
          </template>
          <template slot-scope="{row,index}" slot="state">
            <span>{{renderState(row.state)}}</span>
          </template>

          <template slot-scope="{ row, index }" slot="action">
            <Poptip confirm :transfer="true" title="确定要通过吗?" @on-ok="handleAudit(row,1)">
              <Tooltip placement="top" content="通过" :delay="1000" :transfer="true">
                <Button v-can="'audit'" type="success" size="small" shape="circle" icon="md-checkmark" v-if="row.state==0"></Button>
              </Tooltip>
            </Poptip>
            <Poptip confirm :transfer="true" title="确定不通过吗?" @on-ok="handleAudit(row,2)">
              <Tooltip placement="top" content="不通过" :delay="1000" :transfer="true">
                <Button v-can="'audit'" type="error" size="small" shape="circle" icon="md-close" v-if="row.state==0"></Button>
              </Tooltip>
            </Poptip>
          </template>
        </Table>
      </dz-table>
    </Card>
  </div>
</template>
<script>
  import DzTable from "_c/tables/dz-table.vue";
  import Tables from "_c/tables";
  import {
    getPostjobsAppealList,
    doAppealPass,
    doAppealNoPass
  } from "@/api/workstudy/postjobsappeal";
  export default {
    name: "postjobsappeal",
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
              state:0,
              status: -1,
              sort: [
                {
                  direct: "DESC",
                  field: "ID"
                }
              ]
            },
            sources: {
              stateSources: [
                { value: -1, text: "全部" },
                { value: 0, text: "待审核" },
                { value: 1, text: "通过" },
                { value: 2, text: "未通过" }
              ],
            },
            columns: [
              { type: "selection", width: 50, key: "handle" },
              { title: "用工单位", key: "unit",ellipsis: true },
              { title: "岗位名称", key: "unitName",ellipsis: true },
              { title: "学生姓名", key: "stuName",ellipsis: true },
              { title: "性别", key: "sex",ellipsis: true },
              { title: "年级", key: "grade",ellipsis: true },
              { title: "班级", key: "class",ellipsis: true },
              { title: "监护人姓名", key: "guardianName",ellipsis: true },
              { title: "监护人手机", key: "guardianPhone",ellipsis: true },
              { title: "是否贫困生", key: "poorState",slot:"poorState"},
              // { title: "是否启用", key: "isEnable"},
              { title: "审核状态", key: "state",slot:"state" },
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
      renderPoorState(isEnable){
        let _status = "未知";
        switch(isEnable){
          case 0:
            _status= "是";
            break;
          case 1:
            _status= "否";
            break;
        }
        return _status;
      },
      renderState(isEnable){
        let _status = "未知";
        switch(isEnable){
          case 0:
            _status= "待审核";
            break;
          case 1:
            _status= "通过";
            break;
          case 2:
            _status= "未通过";
            break;
        }
        return _status;
      },
      loadpostjobsAppealList() {
        getPostjobsAppealList(this.stores.postjobs.query).then(res => {
          this.stores.postjobs.data = res.data.data;
          this.stores.postjobs.query.totalCount = res.data.totalCount;
        });
      },
      handleSearchPostjobs() {
        this.stores.postjobs.query.currentPage = 1;
        this.loadpostjobsAppealList();
      },
      handleRefresh() {
        this.stores.postjobs.query.currentPage = 1;
        this.loadpostjobsAppealList();
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
      handlePageChanged(page) {
        this.stores.postjobs.query.currentPage = page;
        this.loadpostjobsAppealList();
      },
      handlePageSizeChanged(pageSize) {
        this.stores.postjobs.query.pageSize = pageSize;
        this.loadpostjobsAppealList();
      },
      handleAudit(row,state)
      {
        if(state==1)
        {
          doAppealPass({guid:row.postJobsAppealUuid}).then(res=>{
            if(res.data.code==200)
            {
              this.$Message.success(res.data.message);
              this.loadpostjobsAppealList();
            }
            else
            {
              this.$Message.warning(res.data.message);
            }
          })
        }
        else if(state==2)
        {
          doAppealNoPass({guid:row.postJobsAppealUuid}).then(res=>{
            if(res.data.code==200)
            {
              this.$Message.success(res.data.message);
              this.loadpostjobsAppealList();
            }
            else
            {
              this.$Message.warning(res.data.message);
            }
          })
        }
      }

    },
    mounted() {
      this.loadpostjobsAppealList();
    }
  };
</script>
<style scoped>
</style>
