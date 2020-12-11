<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.cuisine.query.totalCount"
        :pageSize="stores.cuisine.query.pageSize"
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
                      style="width: 150px;"
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.cuisine.query.kw1"
                      placeholder="请输入标题"
                      @on-search="handleSearchDispatch()"
                    ></Input>
                  </FormItem>
                </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                  <Button
                    v-can="'batch'"
                     v-show="schoolshow1==1"
                    class="txt-danger"
                    icon="md-trash"
                    title="批量删除"
                    @click="handleBatchCommand('delete')"
                  ></Button>
                  <Button   icon="md-refresh" title="刷新" @click="handleRefresh"></Button>
                </ButtonGroup>
                <Button
                  v-can="'create'"
                   v-show="schoolshow1==1"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="添加"
                >添加</Button>
                <!-- <Button
                  v-can="'import'"
                  icon="ios-cloud-upload"
                  type="success"
                  @click="handleImportCuisine"
                  title="导入"
                >导入</Button>
                <Button
                  v-can="'export'"
                  icon="ios-cloud-download-outline"
                  type="primary"
                  @click="handleExportCuisine('export')"
                  title="导出"
                >导出</Button> -->
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
          :data="stores.cuisine.data"
          :columns="stores.cuisine.columns"
          @on-select="handleSelect"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
        >
          <!-- <template slot-scope="{row, index}" slot="auditState">
            <span>{{renderAuditState(row.auditState)}}</span>
          </template>-->
          <template slot-scope="{ row, index }" slot="action">
            <Poptip confirm :transfer="true" title="确定要删除吗?" @on-ok="handleDelete(row)">
              <Tooltip placement="top" content="删除" :delay="1000" :transfer="true"  v-show="schoolshow1==1">
                <Button type="error" v-can="'delete'" size="small" shape="circle" icon="md-trash"></Button>
              </Tooltip>
            </Poptip>
            <Tooltip placement="top" content="编辑" :delay="1000" :transfer="true"  v-show="schoolshow1==1">
              <Button
                v-can="'edit'"
                type="primary"
                size="small"
                shape="circle"
                icon="md-create"
                @click="handleEdit(row)"
              ></Button>
            </Tooltip>
            <Tooltip placement="top" content="详情" :delay="1000" :transfer="true">
              <Button
                v-can="'show'"
                type="warning"
                size="small"
                shape="circle"
                icon="md-search"
                @click="handleDetail(row)"
              ></Button>
            </Tooltip>
          </template>
        </Table>
      </dz-table>
    </Card>
    <Drawer
      :title="formTitle"
      v-model="formModel.opened"
      width="400"
      :mask-closable="false"
      :mask="true"
    >
      <Form
        :model="formModel.fields"
        ref="formdispatch"
        :rules="formModel.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <FormItem label="标题" prop="headline">
            <Input v-model="formModel.fields.headline" placeholder="请输入标题"  type="textarea" :readonly="this.formModel.mode === 'show'" />
          </FormItem>
        </Row>         
        <Row :gutter="16">
          <quill-editor
            @focus="onEditorFocus($event)"
            class="mec"
            v-model="formModel.fields.content"
          ></quill-editor>
          <!-- <FormItem label="简介" prop="content">
            <Input v-model="formModel.fields.content" placeholder="简介"       :autosize="{minRows: 2,maxRows: 10}" type="textarea"/>
          </FormItem> -->
        </Row>
      </Form>
      <div class="demo-drawer-footer" style="margin-top: 150px;">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitConsumable" v-if="!isreadonly()">保 存</Button>
        <Button style="margin-left: 30px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>
    <Drawer :title="详情" v-model="formModel1.opened" width="400" :mask-closable="false" :mask="false">
      <Form :model="formModel1.fields" ref="formdispatch" label-position="left">
        
        <Row :gutter="16">
          <FormItem label="培训主题">
            <Input v-model="formModel1.fields.headline" :readonly="true" type="textarea"/>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="培训时间">
            <Input v-model="formModel1.fields.addTime" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="简介" prop="content">
            <Input v-model="formModel1.fields.content" :autosize="true" :readonly="true" type="textarea"/>
          </FormItem>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button style="margin-left: 30px" icon="md-close" @click="formModel1.opened = false">取 消</Button>
      </div>
    </Drawer>
  </div>
</template>
<script>
import DzTable from "_c/tables/dz-table.vue";
import {
 ManagePlanList,
 Create,
 Delete,
 Batch,
 Edit,
 PlanGet,
} from "@/api/plan/plan";
import config from "@/config";
export default {
  name: "managePlan_page",
  components: {
    DzTable
  },
  data() {
    return {
      commands: {
        delete: { name: "delete", title: "删除" },
        recover: { name: "recover", title: "恢复" },
        Import: { name: "Import", title: "导入" },
        export: { name: "export", title: "导出" }
      },
      schoolshow1:0,
      inglist:[],
      model1: [],
      model2: [],
      stores: {
        cuisine: {
          query: {
            totalCount: 0,
            pageSize: 20,
             //d定义11
              schoolguid:"",
            currentPage: 1,
            kw: "",
            kw1: "",
            isDelete: 0,
            status: -1,
            sort: [
              {
                direct: "DESC",
                field: "ID"
              }
            ]
          },
          sources: {},
          //列表显示
          columns: [
            { type: "selection", width: 50, key: "manageUuid" },
            { title: "标题", key: "headline" , sortable: true},
            { title: "时间", key: "addTime" },
            // { title: "简介", key: "content" ,ellipsis:true},
            {
              title: "操作",
              align: "center",
              width: 200,
              className: "table-command-column",
              slot: "action"
            }
          ],
          data: []
        }
      },
      formModel: {
        opened: false,
        title: "创建申请",
        mode: "create",
        selection: [],
        fields: {
          manageUuid: "",
          headline:"",
          schoolUuid:"",
          addTime:"",       
          content:"",
          isDeleted: 0,
        },
        type: "荤菜",
        cuisinetype: [
          { type: "荤菜" },
          { type: "半荤菜" },
          { type: "素菜" },
          { type: "甜点" },
          { type: "其它" }
        ],
        rules: {
          // cuisineName: [
          //   { type: "string", required: true, message: "请输入菜品名称" }
          // ]
        }
      },
      formModel1: {
        opened: false,
        selection: [],
        fields: {
         manageUuid: "",
          headline:"",
          schoolUuid:"",
          addTime:"",       
          content:"",
          isDeleted: 0,
        }
      }
    };
  },
  computed: {
    formTitle() {
      if (this.formModel.mode === "create") {
        return "新增信息";
      }
      if (this.formModel.mode === "edit") {
        return "编辑信息";
      }
      if (this.formModel.mode === "show") {
        return "详情";
      }
      return "";
    },
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map(x => x.manageUuid);
    } //删除
  },
  methods: {
    //页面加载
    loadDispatchList() {
       if (this.$store.state.user.schoolguid == null) {
                this.schoolshow1=0;
                
      }else{
         this.schoolshow1=1;
      }
       //赋值222
        this.stores.cuisine.query.schoolguid = this.$store.state.user.schoolguid;
      ManagePlanList(this.stores.cuisine.query).then(res => {
        this.stores.cuisine.data = res.data.data;
        this.stores.cuisine.query.totalCount = res.data.totalCount;
      });
    },
    handleSelect(selection, row) {},
    //多选 
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    //翻页
    handlePageChanged(page) {
      this.stores.cuisine.query.currentPage = page;
      this.loadDispatchList();
    },
    //显示条数改变
    handlePageSizeChanged(pageSize) {
      this.stores.cuisine.query.pageSize = pageSize;
      this.loadDispatchList();
    },
    //行样式
    rowClsRender(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },
    //搜索
    handleSearchDispatch() {
      this.loadDispatchList();
    },
    checkShow() {
      return this.formModel.mode === "show"||this.formModel.mode === "edit";
    },
    isreadonly(){
      return this.formModel.mode === "show";
    },
    //刷新
    handleRefresh() {
      this.loadDispatchList();
    },
    //清空
    handleResetFormDispatch() {
      this.$refs["formdispatch"].resetFields();
      this.formModel.fields.content = "";
      this.formModel.fields.headline = "";
      this.formModel.fields.addTime = "";
      this.formModel.fields.schoolUuid = "";
    },
    //右边删除（单个删除）
    handleDelete(row) {
      this.doDelete(row.manageUuid);
    },
    doDelete(ids) {
      if (!ids) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      Delete(ids).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadDispatchList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //右上边删除按钮
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
    },
    //右上边删除
    doBatchCommand(command) {
      Batch({
        command: command,
        ids: this.selectedRowsId.join(",")
      }).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadDispatchList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },
    //类别选择
    changetype(e) {
      this.formModel.fields.cuisineType = e;
    },
        //食材
    doIngredientGet(guid){
      PlanGet(guid).then(res=>{
        this.formModel.fields = res.data.data;
        this.formModel1.fields = res.data.data;
        console.log(this.formModel.fields)
      //this.inglist = res.data.data;
      })
    },
    //添加按钮
    handleShowCreateWindow() {
      this.formModel.mode = "create";        
      this.handleResetFormDispatch(); //清空表单
      this.formModel.opened = true;

    },
    //右边编辑
    handleEdit(row) {
      this.formModel.mode = "edit";
      this.formModel.opened = true;
      this.handleResetFormDispatch(); //清空表单
      //this.doLoadData(row.trainUuid);
      this.doIngredientGet(row.manageUuid);

    },
    //右边详情
    handleDetail(row) {
      this.formModel.mode = "show";

      this.formModel.opened = true;
      this.handleResetFormDispatch(); //清空表单
       this.doIngredientGet(row.manageUuid);
             
      // this.doLoadData(row.trainUuid);
    },
    //查询当前行信息
    doLoadData(id) {
      CuisineGet(id).then(res => {
        if (res.data.code === 200) {
          this.formModel.fields = res.data.data[0];
          this.formModel1.fields = res.data.data[0];        
        }
      });
    },
    //保存按钮
    handleSubmitConsumable() {
      if (this.formModel.fields.headline=="") {
        this.$Message.warning("请填写标题");
        return;
      }
      if (this.formModel.fields.content=="") {
        this.$Message.warning("请填写简介");
        return;
      }
       if (this.formModel.mode === "create") {
        this.docreateDispatch();
      }
      if (this.formModel.mode === "edit") {
        this.doEditDispatch();
      }
    },
    //添加（保存）
    docreateDispatch() {
      this.formModel.fields.schoolUuid=this.$store.state.user.schoolguid;
      Create(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel.opened = false; //关闭表单
          this.loadDispatchList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
       //this.$refs["formdispatch"].resetFields();
    },
    //编辑（保存）
    doEditDispatch() { 
      Edit(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel.opened = false; //关闭表单
          this.loadDispatchList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    onEditorFocus(event) {
      if (this.formModel.mode === "show") {
        event.enable(false);
      } else {
        event.enable(true);
      }
    },
  },
  mounted() {
    this.loadDispatchList(); //页面加载
  }
};
</script>
<style>
.mec {
  height: 400px;
}
</style>